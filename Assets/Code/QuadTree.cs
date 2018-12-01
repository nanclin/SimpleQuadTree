using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuadTree {

    private QuadTree Root;

    public QuadTree GetRoot { get { return Root == null ? this : Root; } }

    public QuadTree[] SubTrees;
    public int Depth;
    public float TreeHeight;
    public Vector2 Position;
    public float Size;
    public float Value;

    public QuadTree(Vector2 pos, float size, float value, float treeHeight, QuadTree root = null, int depth = 0) {
        Position = pos;
        Size = size;
        Root = root;
        TreeHeight = treeHeight;
        Depth = depth;
    }

    public void SetValue(Vector2 pos, float value, QuadTree root = null, int depth = 0) {
        // reached leaf
        if (depth >= root.TreeHeight) {
            Value = value;
            return;
        }

        float x = pos.x;
        float y = pos.y;

        // split
        float hs = this.Size * 0.5f; // half size
        if (SubTrees == null) {
            SubTrees = new QuadTree[4];
            Vector2 newPos0 = this.Position + new Vector2(hs, hs);
            Vector2 newPos1 = this.Position + new Vector2(0, hs);
            Vector2 newPos2 = this.Position + new Vector2(0, 0);
            Vector2 newPos3 = this.Position + new Vector2(hs, 0);
            SubTrees[0] = new QuadTree(newPos0, hs, 0, TreeHeight - 1, root, depth + 1);
            SubTrees[1] = new QuadTree(newPos1, hs, 0, TreeHeight - 1, root, depth + 1);
            SubTrees[2] = new QuadTree(newPos2, hs, 0, TreeHeight - 1, root, depth + 1);
            SubTrees[3] = new QuadTree(newPos3, hs, 0, TreeHeight - 1, root, depth + 1);
        }

        QuadTree nextSubTree = null;
        float cx = this.Position.x + hs; // center x
        float cy = this.Position.y + hs; // center y
        if (x > cx && y > cy) {
            // quadrant 1
            nextSubTree = SubTrees[0];
        } else if (x <= cx && y > cy) {
            // quadrant 2
            nextSubTree = SubTrees[1];
        } else if (x <= cx && y <= cy) {
            // quadrant 3
            nextSubTree = SubTrees[2];
        } else if (x > cx && y <= cy) {
            // quadrant 4
            nextSubTree = SubTrees[3];
        }
        nextSubTree.SetValue(pos, value, root, depth + 1);
    }

    public float GetValue(Vector2 pos, QuadTree root = null, int depth = 0) {
        // reached leaf
        if (depth >= root.TreeHeight || this.SubTrees == null) {
            return Value;
        }

        QuadTree nextSubTree = null;
        float x = pos.x;
        float y = pos.y;
        float hs = this.Size * 0.5f; // half size
        float cx = this.Position.x + hs; // center x
        float cy = this.Position.y + hs; // center y
        if (x > cx && y > cy) {
            // quadrant 1
            nextSubTree = SubTrees[0];
        } else if (x <= cx && y > cy) {
            // quadrant 2
            nextSubTree = SubTrees[1];
        } else if (x <= cx && y <= cy) {
            // quadrant 3
            nextSubTree = SubTrees[2];
        } else if (x > cx && y <= cy) {
            // quadrant 4
            nextSubTree = SubTrees[3];
        }
        return nextSubTree.GetValue(pos, root, depth + 1);
    }
}
