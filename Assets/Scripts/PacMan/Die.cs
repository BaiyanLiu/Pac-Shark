using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PacMan
{
    public class Die : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SceneManager.LoadScene("Game Over", LoadSceneMode.Additive);
        }
    }
}
