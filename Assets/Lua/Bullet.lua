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


local Vector3 = CS.UnityEngine.Vector3

function Bullet.GetFireDirection(maxRad)
    local cosMax = math.cos(maxRad)
    local cosTheta = CS.UnityEngine.Random.Range(cosMax, 1)
    local sinTheta = math.sqrt(1 - cosTheta * cosTheta)
    local phi = CS.UnityEngine.Random.Range(0, 2 * math.pi)

    return Vector3(sinTheta * math.cos(phi), sinTheta * math.sin(phi), cosTheta)
end

return Bullet