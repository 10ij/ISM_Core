namespace ISM.Core;

[Flags]
public enum DaysOfWeek
{
    None = 0, // 0000 0000
    Monday = 1, // 0000 0001
    Tuesday = 2, // 0000 0010
    Wednesday = 4, // 0000 0100
    Thursday = 8, // 0000 1000
    Friday = 16, // 0001 0000
    Saturday = 32, // 0010 0000
    Sunday = 64 // 0100 0000

    // e.g. mon and tues will be 0000 0011 which is 3 in denary

}

public enum PersonTitle
{
    Mr = 0,
    Mrs = 1,
    Miss = 2,
    Ms = 3,
    Dr = 4,
    Ust = 5
}

public enum Gender
{
    Male = 0,
    Female = 1
}

public enum LedgerEntryType
{
    Due = 0,
    Payment = 1,
    Refund = 2,
    Exemption = 3,
    WrittenOff = 4
}

public enum Relation
{
    Father = 0,
    Mother = 1,
    Guardian = 2,
    Brother = 3,
    Sister = 4,
    Uncle = 5,
    Aunt = 6,
    Grandfather = 7,
    Grandmother = 8,
    Other = 9
}