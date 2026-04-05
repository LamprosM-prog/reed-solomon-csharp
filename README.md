# Reed-Solomon Encoder/Decoder (GF(256))

- A full implementation of Reed-Solomon error correction over GF(256), built from scratch in C#.
This project demonstrates how messages can be encoded, corrupted, and then perfectly recovered using classical error-correcting algorithms.

## Features
- Encoding using generator polynomials in GF(256)
- Error detection via syndrome computation
- Error locator polynomial using Berlekamp-Massey
- Error position detection using Chien Search
- Error magnitude computation using Forney’s Algorithm
- Automatic correction of corrupted codewords
- Human-readable outputs:
 - Byte representation
 - Polynomial form
 - String representation
## Pipeline
- Encode → Corrupt → Syndromes → Berlekamp-Massey → Chien Search → Forney → Corrected
## Explanations
-GF256.cs -> This is where the galois field is generated and where we define the properties of it. (Ex. In the galois field addition is the xor operation)
-Polynomial.cs -> This is where we define the polynomial operations. All use the GF256 operations and work similarly to polynomials over all real numbers.
 -NOTE: The derivative function is mathematically sound only IN THE GALOIS FIELD, it doesn't follow the same logic in other fields/sets.
-Generator.cs -> This is to generate the inductive polynomial that will be used later in the pipeline to calculate the Error-Correction-Codes and append them to the message.
-Encoder.cs -> Here is where the titular algorithm happens. With the help of the inductive polynomial we calculate the ECCs and append them TO THE END of the message.
 -Note: In other implementations the ECC symbols may be placed differently.
-Decoder.cs -> The most complex part of this program/library, this is where syndrome computation , error detection and error-correction happen. For this I will go over each function one by one.
 -ComputeSyndromes -> Self-explanatory, uses the prime element (Helper(i) returns α^i, the i-th power of the primitive element α=2 with i in  [0,511] (511 because the exponent table is actually doubled so that multiplication via log tables never needs modulo reduction mid-lookup)
 -CheckForErrors -> Checks if the syndromes are 0 0 0 0, if they are no errors have occured
 -BerlekampMassey -> This algorithm is too complex for me to explain in it's entirety  here, refer to the specific document about it. Simplified, it creates the error-locator polynomial (Λ(x)), which will be needed later.
  -NOTE: Any changes to this algorithm if done poorly, will result in very obsucure and hard to find bugs. If changes are done , remember that the constant term of lambda must always be 1 and the degree must always be the same as the number of errors.
 -ChienSearch -> Using Horner's Scheme this will find the roots of Λ(x) and maps them to the codeword positions. This works because the degree of Λ(x) equals to the number of errors that have occured
 -Forney -> This algorithm finds the Error-Magnitudes (The difference between the original and corrupted values) and corrects them. Forney uses the error evaluator polynomial Ω(x) = S(x)*Λ(x) mod x^(2t) 
and the formal derivative Λ'(x) to compute the magnitude.
## Notes
- The random noise generator knows how many errors you have selected and will not create more errors than can be corrected.
- In real systems if the number of errors exceed the ECC length some errors will not be correctable.
- Non-printable characters may appear in corrupted string output
- Maximum supported errors in this implementation: 64 (for performance reasons). 
   *Theoretical limit is 127, due to the usage of 2t ECC symbols and the max codeword legth being 255. 
- This implementation is fully done from scratch without the use of external libraries.
- This program is framework dependent and will require .NET10 to run
## Real World Applications
Reed-Solomon codes are used in:
- CDs and DVDs
- QR codes
- Deep space communication (Voyager, Cassini)
- RAID storage systems
## How to Run
Type in your console:
dotnet run --project ReedSolomonConsole
