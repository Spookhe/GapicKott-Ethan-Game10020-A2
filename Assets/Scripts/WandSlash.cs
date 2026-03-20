/* Ethan Gapic-Kott, 000923124 */

using UnityEngine;
using System.Collections;

public class WandSlash : MonoBehaviour
{
    public float slashAngle = 60f;
    public float slashSpeed = 8f;

    Quaternion startRot;

    void Start()
    {
        startRot = transform.localRotation;
    }

    // Restarts wand animations
    public void PlaySlash()
    {
        StopAllCoroutines();
        StartCoroutine(Slash());
    }

    IEnumerator Slash()
    {
        Quaternion down = Quaternion.Euler(slashAngle, 0, 0) * startRot;

        float t = 0;

        // Allows wands to play an animation
        while (t < 1)
        {
            t += Time.deltaTime * slashSpeed;
            transform.localRotation = Quaternion.Lerp(startRot, down, t);
            yield return null;
        }

        t = 0;

        // Returns the wand to a static unanimated state
        while (t < 1)
        {
            t += Time.deltaTime * slashSpeed;
            transform.localRotation = Quaternion.Lerp(down, startRot, t);
            yield return null;
        }
    }
}