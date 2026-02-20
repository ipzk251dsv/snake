using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

const int FIELD = 12;

var renderer = (IRenderer)new ConsoleRenderer();
var inputProvider = (IInputProvider)new ConsoleInputProvider();

var food = new Vector2();
var snake = new List<Vector2>() {
  new Vector2()
};
var direction = new Vector2() {
  X = 1
};
var speed = 1.0f;

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
  renderer.Clear();
  renderer.ShowMessage($"Your score: {snake.Count}");
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
  var key = inputProvider.GetKey();
  if (key == null) return;

  var input = key switch {
    Key.UpArrow => new Vector2() { Y = -1 },
    Key.DownArrow => new Vector2() { Y = 1 },
    Key.RightArrow => new Vector2() { X = 1 },
    Key.LeftArrow => new Vector2() { X = -1 },
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
  renderer.Clear();

  renderer.DrawPoint(Color.Red, food.X + 1, food.Y + 1);

  foreach (var part in snake) renderer.DrawPoint(Color.Green, part.X + 1, part.Y + 1);

  for (var x = 0; x < FIELD + 2; ++x) renderer.DrawPoint(Color.DarkGray, x, 0);

  for (var y = 0; y < FIELD + 2; ++y)
  {
    renderer.DrawPoint(Color.DarkGray, 0, y);
    renderer.DrawPoint(Color.DarkGray, FIELD + 1, y);
  }

  for (var x = 0; x < FIELD + 2; ++x) renderer.DrawPoint(Color.DarkGray, x, FIELD + 1);
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