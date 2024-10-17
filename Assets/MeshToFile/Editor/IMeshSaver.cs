using UnityEngine;

namespace Pinwheel.MeshToFile
{
    public interface IMeshSaver
    {
        void Save(Mesh m, Material mat, string path);
    }
}