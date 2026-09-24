local Rifle = {
    maxMagazineSize = 30, --最大弹容量
    duration = 0.4, --枪口复位时间
    fireInterval = 0.07, --开火间隔时间
    maxAngleDeg = 2, --最大散射角度
    
    startAngle = 0, --当前后坐力角度
    targetOffset = 2 --后坐力威力
}

function Rifle.GetRecoil(elapsed)

    local t = elapsed / Rifle.duration

    if t < 0.3 then --上升
        local recoilT = t / 0.3
        local smoothT = recoilT * recoilT * (3 - 2 * recoilT)
        return CS.UnityEngine.Mathf.Lerp(Rifle.startAngle, Rifle.startAngle - Rifle.targetOffset, smoothT)
    else --下降
        local recoilT = (t - 0.3) / 0.7
        local smoothT = recoilT * recoilT * (3 - 2 * recoilT)
        return CS.UnityEngine.Mathf.Lerp(Rifle.startAngle - Rifle.targetOffset, Rifle.startAngle, smoothT)
    end

end

return Rifle