
public enum InputType
{
    Separated,
    Combinated,
    Mouse
}


public static class PlayerSettings
{
    private static InputType inputType;

    public static void SetInputType(InputType input)
    {
        inputType = input;
    }

    public static InputType GetInputType()
    {
        return inputType;
    }
}
