using System;
using System.Collections.Generic;

namespace TimeWheel;
/// <summary>
/// 轮盘槽
/// </summary>
public class TimeWheelSlot {
    // 存储各个槽中的定时任务
    public LinkedList<Action> Tasks { get; } = new LinkedList<Action>();
}