
public enum InputType
{
    Separated,
    Combinated,
    Mouse
}

public static class PlayerSettings
{
    private static InputType inputType = InputType.Combinated;

    public static void SetInputType(InputType input)
    {
        inputType = input;
    }

    public static InputType GetInputType()
    {
        return inputType;
    }
}
