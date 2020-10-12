A service exists that provides numerical data from a pair of two-dimensional datasets A and B. The contents and dimensions of A and B can be interpreted as two 2D square matrices, which when multiplied together produce a third matrix that is the desired result.
This is a console program that retrieves the datasets A & B, multiplies their matrix representations (A X B), and submits the result back to the service.

    The service API description at https://recruitment-test.investcloud.com/.
    
    Initialize the dataset size to 1000 x 1000 elements. 
    
    The result matrix formatted as a concatenated string of the matrix' contents (left-to-right, top-to-bottom), hashed using the md5 algorithm. Submit the md5 hash to validate your result and receive a passphrase from the service indicating success or failure.
    
    Total runtime should be as fast as possible, given the size of the datasets, the nature of the service API, and the mathematical operation requested (cross product of 2 matrices)

    

Reference for Matrix Multiplication (A X B):

![Reference](/wQz_PvB1Btr7X6063gAkh_oTpDCldjOiFsUGi7KcHDCo1JKZERWl9yHAny_FDL0XSDjdxjB2cFWLeQWajfX5VemDQG3pvfOL8y4OQX-RkDadpagQC6tiHvADMGzqaGdhkD3eWCVe.png)
