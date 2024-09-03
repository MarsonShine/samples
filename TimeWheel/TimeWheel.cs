using System;
using System.Threading;

namespace TimeWheel;

public class TimeWheel {
    private readonly int _slotCount;
    private int _currentSlot;
    private readonly TimeWheelSlot[] _slots;
    private readonly TimeSpan _tickDuration;
    private readonly Timer _timer;
    public TimeWheel(int slotCount, TimeSpan tickDuration) {
        _slotCount = slotCount;
        _slots = new TimeWheelSlot[slotCount];
        // 初始化槽
        for (int i = 0; i < slotCount; i++) {
            _slots[i] = new TimeWheelSlot();
        }
        _tickDuration = tickDuration;
        // 初始化定时器
        _timer = new Timer(OnTick, null, tickDuration, tickDuration);
    }

    private void OnTick(object? state) {
        var currentSlot = _slots[_currentSlot];
        foreach (var task in currentSlot.Tasks) {
            task.Invoke();
        }
        // 清空当前槽
        currentSlot.Tasks.Clear();
        _currentSlot = (_currentSlot + 1) % _slotCount;
    }

    public void AddTask(Action task, TimeSpan delay) {
        // 计算延迟需要的时钟周期数
        int delayTicks = (int)(delay.TotalMilliseconds / _tickDuration.TotalMilliseconds);
        
        // 计算任务应该被放置的槽索引
        // 当前槽位加上延迟的时钟周期数，然后对总槽数取模，确保索引在有效范围内
        int slotIndex = (_currentSlot + delayTicks) % _slotCount;
        
        // 将任务添加到计算出的槽位中
        _slots[slotIndex].Tasks.AddLast(task);
    }

    public void Stop() {
        _timer.Dispose();
    }
}