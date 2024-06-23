using System;
using System.Reflection;

public class GameExceptions
{
  public sealed class MissingGameObjectReference : Exception
  {
    public MissingGameObjectReference(string name, Type type) : base($"Reference to {type} missing on {name}.") {}
	}

  public sealed class MissingEventListener : Exception
  {
    public MissingEventListener(string name, string eventName) : base($"Listener for event {eventName} missing on {name}.") { }
  }
}
