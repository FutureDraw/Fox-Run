using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>
// ��������� ��� ���������� ��������� (������, ������� ������, ��������)
// </Summary>
internal interface ITrap
{
    void StopPlayer(float Time);
    void SlowPlayer(float Time, float Strenght);
    void KillPlayer();

}
