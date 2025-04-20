using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>
// Интерфейс для управления ловушками (капкан, летящая стрела, хваталка)
// </Summary>
internal interface ITrap
{
    void StopPlayer(float Time);
    void SlowPlayer(float Time, float Strenght);
    void KillPlayer();

}
