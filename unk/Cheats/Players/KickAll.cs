using Photon.Pun;
using unk.Cheats.Core;

namespace unk.Cheats
{
    internal class KickAll : ExecutableCheat
    {
        public static void Execute()
        {
            PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

    }
}
