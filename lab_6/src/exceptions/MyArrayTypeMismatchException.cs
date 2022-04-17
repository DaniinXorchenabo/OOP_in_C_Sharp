using System;

namespace lab_6.exceptions;

public class MyArrayTypeMismatchException: ArrayTypeMismatchException
{
    public MyArrayTypeMismatchException(string message):
        base(@"Ошибка из моего исключения со следующим текстом: " + message)
    {
        
    }
}