public class ContainerBase
{
    public ContainerBase() => InstanceCountBase++;
    public static int InstanceCountBase { get; private set; }

}