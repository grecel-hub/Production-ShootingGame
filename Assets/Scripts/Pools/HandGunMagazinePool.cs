//泛型对象池挂载脚本
public class HandGunMagazinePool : ObjectPool<Magazine>
{
    public static HandGunMagazinePool instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    protected override void Start()
    {
        base.Start();
    }

}
