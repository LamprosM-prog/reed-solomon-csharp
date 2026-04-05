# Berlekamp-Massey Algorithm

## Overview
The Berlekamp-Massey algorithm is an iterative algorithm that finds the shortest 
Linear Feedback Shift Register (LFSR) that generates a given sequence. 
In the context of Reed-Solomon decoding, it is used to find the error locator 
polynomial Λ(x), whose roots correspond to the positions of errors in the codeword.

## Key Invariants
- Λ(0) = 1 always (constant term is always 1)
- degree of Λ(x) = number of errors detected
- If these invariants are violated, the algorithm has been implemented incorrectly

## Variables
- `lambda` — the current error locator polynomial Λ(x), stored with lowest index = constant term
- `bestL` — the best previous version of lambda, used to update lambda when a non-zero discrepancy is found
- `b` — the discrepancy value at the time bestL was last updated, used to scale corrections
- `degree` — current degree of Λ(x), equals the number of errors found so far
- `m` — the iteration at which bestL was last updated
- `k` — current iteration (0 to 2t-1)
- `delta` — the discrepancy at iteration k, measures how well the current Λ(x) predicts the syndrome sequence

## Algorithm Steps

### 1. Compute Discrepancy
At each iteration k, compute the discrepancy delta:
- δ = S[k] + Λ[1]*S[k-1] + Λ[2]*S[k-2] + ... + Λ[L]*S[k-L]
If delta is 0, the current Λ(x) already predicts S[k] correctly — no update needed.

### 2. Update Lambda
If delta is non-zero, update Λ(x):
- Λ(x) = Λ(x) + (δ/b) * x^(k-m) * bestL(x)
The `(δ/b)` scaling ensures the correction is properly normalized relative to 
when bestL was last saved.

### 3. Update Degree and bestL
If `2 * degree <= k`, the degree of Λ(x) must increase:
- new degree = k + 1 - degree
- bestL = previous lambda (before this iteration's update)
- b = delta
- m = k
### 4. Convention Note
This implementation stores lambda with **lowest index = constant term** internally.
Before returning, lambda is trimmed and reversed so that it follows the 
**big-endian convention** (highest index = constant term) used by the rest of 
the polynomial operations in this library.

## Why This Works
The syndromes of a Reed-Solomon codeword with errors satisfy a linear recurrence 
relation defined by the error locator polynomial. BM finds the shortest such 
recurrence — which is exactly Λ(x). Once found, the roots of Λ(x) reveal the 
error positions via Chien Search.

## Warning
This algorithm is sensitive to implementation details. Common pitfalls:
- Missing the `b = delta` update causes wrong scaling and incorrect lambda coefficients
- Wrong shift direction in the update loop silently drops terms
- Not trimming lambda before reversing causes a leading zero which corrupts Chien Search
- The internal convention (lowest index = constant) must be reversed before returning
