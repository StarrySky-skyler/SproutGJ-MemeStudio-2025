using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 抖动 : MonoBehaviour
{
    public float 抖动范围 = 1f;
    public float 抖动频率 = 1f; // 抖动频率，以秒为单位
    public float 抖动速度 = 1f; // 抖动速度
    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private float timer;

    void Start()
    {
        // 记录初始位置
        initialPosition = transform.position;
        targetPosition = initialPosition;
        timer = 0f;
    }

    void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;

        // 当计时器达到抖动频率时，计算新的目标位置并重置计时器
        if (timer >= 抖动频率)
        {
            // 在抖动范围内计算新的目标位置
            float newX = initialPosition.x + Random.Range(-抖动范围, 抖动范围);
            float newY = initialPosition.y + Random.Range(-抖动范围, 抖动范围);

            // 限制目标位置在初始范围内
            newX = Mathf.Clamp(newX, initialPosition.x - 抖动范围, initialPosition.x + 抖动范围);
            newY = Mathf.Clamp(newY, initialPosition.y - 抖动范围, initialPosition.y + 抖动范围);

            targetPosition = new Vector2(newX, newY);

            // 重置计时器
            timer = 0f;
        }

        // 使用 Lerp 方法平滑地移动到目标位置
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * 抖动速度);
    }
}



