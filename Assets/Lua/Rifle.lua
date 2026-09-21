local Rifle = {
    maxMagazineSize = 30, --最大弹容量
    duration = 0.1, --枪口复位时间
    fireInterval = 0.07, --开火间隔时间
    
    startAngle = 0, --当前后坐力角度
    targetOffset = 1 --后坐力威力
}

function Rifle.GetRecoil(elapsed)
    local t = elapsed / Rifle.duration
    local smoothT = t * t * (3 - 2 * t)
    return CS.UnityEngine.Mathf.Lerp(Rifle.startAngle, Rifle.startAngle - Rifle.targetOffset, smoothT)
end

return Rifle