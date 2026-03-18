using System;
using System.LINQ;

public static class Polynomial    
{
	public static byte[] Add(byte[] a , byte[] b)
    {
        a = a.OrderByDecsending(c => c).ToArray();
        b = b.OrderByDecsending(c => c).ToArray();
        
    }

    
}
