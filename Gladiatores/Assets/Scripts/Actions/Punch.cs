using UnityEngine;

public class Punch : Weapon {

    protected override void Start() {
        attackedReach_ = 1.0f;
        weakToType_ = WeaponType.Sword;
        strengthToType_ = WeaponType.Shield;
        thisType_ = WeaponType.Punch;
        base.Start();
    }

    public override void Attack(float InputValue) {
        var foward = transform.parent.parent.parent.parent.up;
        transform.parent.localPosition = (Vector3.MoveTowards(Vector3.zero, foward, InputValue) * 1F);
    }
}
