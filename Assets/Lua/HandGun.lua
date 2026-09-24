local HandGun = {
    maxMagazineSize = 7, --最大弹容量
    duration = 0.4, --枪口复位时间
    fireInterval = 0.1, --开火间隔时间
    maxAngleDeg = 4, --最大散射角度

    startAngle = 0, --当前后坐力角度
    targetOffset = 4 --后坐力威力
}

function HandGun.GetRecoil(elapsed)

    local t = elapsed / HandGun.duration

    if t < 0.3 then --上升
        local recoilT = t / 0.3
        local smoothT = recoilT * recoilT * (3 - 2 * recoilT)
        return CS.UnityEngine.Mathf.Lerp(HandGun.startAngle, HandGun.startAngle - HandGun.targetOffset, smoothT)
    else --下降
        local recoilT = (t - 0.3) / 0.7
        local smoothT = recoilT * recoilT * (3 - 2 * recoilT)
        return CS.UnityEngine.Mathf.Lerp(HandGun.startAngle - HandGun.targetOffset, HandGun.startAngle, smoothT)
    end

end

return HandGun