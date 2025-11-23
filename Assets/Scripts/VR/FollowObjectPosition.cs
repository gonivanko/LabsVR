using UnityEngine;

public class FollowObjectPosition : MonoBehaviour
{
    public Transform target;   // Об’єкт, над яким має бути панель
    public Vector3 offset;     // Наприклад (0, 0.5f, 0) для появи над об’єктом

    private void LateUpdate()
    {
        if (target == null) return;

        // копіюємо тільки позицію
        transform.position = target.position + offset;

        // орієнтуємо панель на камеру (камері видно текст)
        var cam = Camera.main;
        if (cam != null)
        {
            // Повертаємо панель лицем до камери, але не крутимо разом з об’єктом
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180f, 0);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}
