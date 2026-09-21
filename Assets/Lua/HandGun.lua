local HandGun = {
    maxMagazineSize = 7, --最大弹容量
    duration = 0.1, --枪口复位时间
    fireInterval = 0.1, --开火间隔时间

    startAngle = 0, --当前后坐力角度
    targetOffset = 2 --后坐力威力
}

function HandGun.GetRecoil(elapsed)
    local t = elapsed / HandGun.duration
    local smoothT = t * t * (3 - 2 * t)
    return CS.UnityEngine.Mathf.Lerp(HandGun.startAngle, HandGun.startAngle - HandGun.targetOffset, smoothT)
end

return HandGun