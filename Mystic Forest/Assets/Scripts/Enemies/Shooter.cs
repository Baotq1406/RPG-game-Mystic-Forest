﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate; //oscillate meaning "dao dong"

    private bool isShooting = false;

    private void OnValidate()
    {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }   
        if (burstCount < 1) { burstCount = 1; }
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <=0 ) { bulletMoveSpeed = 0.1f; }
    }

    public void Attack()
    {
        //Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        //GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        //newBullet.transform.right = targetDirection;

        if (!isShooting) { 
            StartCoroutine(ShootRoutine()); 
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        //tính toán các góc cần thiết cho việc phân tán đạn
        TargetConeOfInfluence(out  startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger) { timeBetweenProjectiles = timeBetweenBursts/projectilesPerBurst; }


        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate) { 
                TargetConeOfInfluence(out  startAngle, out currentAngle, out angleStep, out endAngle);
            }

            if (oscillate && i % 2 != 1) { 
                TargetConeOfInfluence(out  startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate) {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);

                //Đặt hướng của viên đạn sao cho đối mặt với mục tiêu
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                //Tăng góc hiện tại cho viên đạn tiếp theo trong đợt bắn
                currentAngle += angleStep;

                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            //Thiết lập lại góc về góc ban đầu cho đợt bắn tiếp theo
            currentAngle = startAngle;

            //yield return new WaitForSeconds(timeBetweenBursts);
            //TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 

            if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
        }

        yield return new WaitForSeconds(restTime);
        isShooting=false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0) 
        {
            //Xác định bước tăng giữa các góc của từng viên đạn.
            angleStep = angleSpread / (projectilesPerBurst - 1);
            //Tính toán một nửa độ phân tán góc.
            halfAngleSpread = angleSpread / 2f;
            //Điều chỉnh góc bắt đầu sang bên trái của góc mục tiêu.
            startAngle = targetAngle - halfAngleSpread;
            //Điều chỉnh góc kết thúc sang bên phải của góc mục tiêu.
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

   private Vector2 FindBulletSpawnPos(float currentAngle)
   {
        //Tính toán tọa độ x của vị trí sinh đạn
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad); //Góc được chuyển đổi từ độ sang radian bằng currentAngle * Mathf.Deg2Rad vì các hàm Mathf.Cos và Mathf.Sin yêu cầu đơn vị là radian
        //Tính toán tọa độ y của vị trí sinh đạn.
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);    

        return pos;
   }
}