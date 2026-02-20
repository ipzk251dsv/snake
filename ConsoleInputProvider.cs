using System;

public class ConsoleInputProvider : IInputProvider
{
  public Key? GetKey()
  {
    if (!Console.KeyAvailable) return null;

    Key key;
    do key = (Key)Console.ReadKey(true).Key;
    while (Console.KeyAvailable);

    return key;
  }
}