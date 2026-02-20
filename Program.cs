using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

const int FIELD = 12;

var food = new Vector2();
var snake = new List<Vector2>() {
  new Vector2()
};
var direction = new Vector2() {
  X = 1
};
var speed = 1.0f;

Console.CursorVisible = false;
RespawnFood();

while (true)
{
  ReadInput();

  if (CanEat())
  {
    RespawnFood();
    Grow();
    speed += 0.01f;
  }

  MoveSnake();

  if (CanDie())
  {
    Loose();
    break;
  }

  Redraw();

  Thread.Sleep((int)Math.Round(250 / speed));
}

void Loose()
{
  Console.Clear();
  Console.ResetColor();
  Console.CursorVisible = true;
  Console.WriteLine($"Your score: {snake.Count}");
  Console.ReadLine();
}

bool CanDie()
{
  var head = snake[0];
  return snake.Skip(1).Any(part => part.Equals(head));
}

void Grow()
{
  var last = snake[snake.Count - 1];
  snake.Add(new Vector2() {
    X = last.X,
    Y = last.Y
  });
}

bool CanEat()
{
  var head = snake[0];
  return head.Equals(food);
}

void ReadInput()
{
  if (!Console.KeyAvailable) return;

  ConsoleKey key;
  do key = Console.ReadKey(true).Key;
  while (Console.KeyAvailable);

  var input = key switch {
    ConsoleKey.UpArrow => new Vector2() { Y = -1 },
    ConsoleKey.DownArrow => new Vector2() { Y = 1 },
    ConsoleKey.RightArrow => new Vector2() { X = 1 },
    ConsoleKey.LeftArrow => new Vector2() { X = -1 },
  };

  if (input.Equals(direction)) return;

  direction = input;
}

void MoveSnake()
{
  var last = snake[snake.Count - 1];
  for (var i = (snake.Count - 1) - 1; i >= 0; --i)
  {
    var part = snake[i];
    last.X = part.X;
    last.Y = part.Y;
    last = part;
  }

  last.X += direction.X;
  last.Y += direction.Y;

  if (last.X >= FIELD) last.X = 0;
  else if (last.X < 0) last.X = FIELD - 1;
  if (last.Y >= FIELD) last.Y = 0;
  else if (last.Y < 0) last.Y = FIELD - 1;
}

void RespawnFood()
{
  bool done;
  do
  {
    food.X = Random.Shared.Next(0, FIELD);
    food.Y = Random.Shared.Next(0, FIELD);

    done = !snake.Any(part => part.Equals(food));
  }
  while (!done);
}

void Redraw()
{
  Console.Clear();

  Draw(ConsoleColor.Red, food.X + 1, food.Y + 1);

  foreach (var part in snake) Draw(ConsoleColor.Green, part.X + 1, part.Y + 1);

  for (var x = 0; x < FIELD + 2; ++x) Draw(ConsoleColor.DarkGray, x, 0);

  for (var y = 0; y < FIELD + 2; ++y)
  {
    Draw(ConsoleColor.DarkGray, 0, y);
    Draw(ConsoleColor.DarkGray, FIELD + 1, y);
  }

  for (var x = 0; x < FIELD + 2; ++x) Draw(ConsoleColor.DarkGray, x, FIELD + 1);
}

void Draw(ConsoleColor color, int x, int y)
{
  Console.SetCursorPosition(x * 2, y);
  Console.ForegroundColor = color;
  Console.Write("██");
}

class Vector2
{
  public int X;
  public int Y;

  public bool Equals(Vector2 other)
  {
    return X == other.X && Y == other.Y;
  }
}