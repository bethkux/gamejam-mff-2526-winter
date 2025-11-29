public static class Dialgoues
{
    private static readonly string[] _DeathDialogue = new string[]
    {
        "Hello",
        "Hello"
    };

    private static readonly string[] _SpiritDialogue = new string[]
    {
        "Hello",
        "Hello"
    };


    public static string GetDeathDialogue(int index) => _DeathDialogue[index];

    public static string GetSpiritDialogue(int index) => _SpiritDialogue[index];
}
