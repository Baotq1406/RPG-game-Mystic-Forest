using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }
      
    /* Singleton<T>. Nếu một phiên bản đã tồn tại, GameObject mới sẽ bị hủy để duy trì một phiên bản duy nhất.
    DontDestroyOnLoad đảm bảo rằng singleton vẫn tồn tại trong quá trình chuyển đổi cảnh trong Unity. */

    protected virtual void Awake() {
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = (T)this;
        }
        
        if (!gameObject.transform.parent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
