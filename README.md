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