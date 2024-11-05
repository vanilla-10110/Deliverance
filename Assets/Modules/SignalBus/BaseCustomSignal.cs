// using deVoid.Utils;
// using UnityEngine;

// public abstract class BaseCustomSignal : MonoBehaviour {
//         protected static BaseCustomSignal Instance {get; private set;}
        
//         protected ASignal signal;

//         private void Awake(){
//         if (Instance != null && Instance != this){
//             Destroy(this);
//         }
//         else {
//             Instance = this;
//         }
        
//         // signal = new ABaseSignal();

//         }



//     //     // DontDestroyOnLoad(ScoreLabel);
//     //     // DontDestroyOnLoad(MainUI);
//     // }
//         public void Connect(Action action) {
//             Instance.AddListener(action);
//         }

// }