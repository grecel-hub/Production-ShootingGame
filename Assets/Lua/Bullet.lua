local Bullet = {
    speed = 10,
    headDamage = 100,
    torsoDamage = 70,
    limbsDamage = 30
}

function Bullet.GetDamage(layerMask)
    if layerMask == 10 then
        return Bullet.headDamage
    elseif layerMask == 11 then
        return Bullet.torsoDamage
    elseif layerMask == 12 then
        return Bullet.limbsDamage
    else
        return 0
    end
end

return Bullet