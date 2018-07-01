public interface IMoveable{

    void MoveLeft(bool keystate);
    void MoveRight(bool keystate);
    void MoveUp(bool keystate);
    void MoveDown(bool keystate);
    void Use(bool keystate);
}

public interface IHealth
{
    void TakeDamage(int amount);
    void IncreaseHealth(int amount);
}

public interface IWeapon
{
    void Primary();
    void Secondary();
}
