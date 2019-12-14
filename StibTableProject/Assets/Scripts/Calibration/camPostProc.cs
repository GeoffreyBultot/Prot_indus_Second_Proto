using UnityEngine;
using System;

[ExecuteInEditMode]
public class camPostProc : MonoBehaviour
{
    //la texture contenant l'info de si il faut dessiner le pixel ou non
    private Texture2D texMustDraw;
    //texture contenant le point correspondant uv.x pour chaque pixel
    //le float uv.x est encodé en 4 octets répartis dans les composants rgba de la couleur du pixel
    private Texture2D texContainingUVX;
    //pareil uv.y
    private Texture2D texContainingUVY;
    
    //les coins et normales du polygone à redessiner, utilisés dans la formule de transposition
    private Vector2 P0, P1, P2, P3;
    private Vector2 N0, N1, N2, N3;
    Vector2[] polygon;
    int pointsnumber;
    //attend de faire la transposition tant que les coins du poly n'ont pas été sélectionnés
    bool startRender;
    
    //référence à l'objet image qui contient le matériel et le shader pour lui envoyer les textures contenant les données
    public GameObject imgobject;


    void Awake()
    {
        //initialise startRender à false car on n'a pas encore les coins du poly
        startRender = false;
    }

    private void Start()
    {
        //initialise le nombre de coins du poly
        pointsnumber = 0;
        // crée les coins vides
        P0 = new Vector2(0, 0);
        P1 = new Vector2(0, 0);
        P2 = new Vector2(0, 0);
        P3 = new Vector2(0, 0);
        //les met dans un tableau de vecteurs pour en faire un polygone
        Vector2[] vs = { P0, P1, P2, P3 };
        polygon = vs;
    }

    //donne la perpendiculaire du vecteur dans le sens des aiguilles d'une montre
    Vector2 PerpendicularClockwise(Vector2 vector2)
    {
        return new Vector2(vector2.y, -vector2.x);
    }

    //donne la perpendiculaire du vecteur dans le sens contraire des aiguilles d'une montre
    Vector2 PerpendicularCounterClockwise(Vector2 vector2)
    {
        return new Vector2(-vector2.y, vector2.x);
    }

    //récupère la position des points placés à l'écran
    void GetPoints()
    {
        //tant qu'il n'y en a pas 4
        if (pointsnumber < 4)
        {
            // au click de la souris
            if (Input.GetMouseButtonDown(0))
            {
                //ajoute le point au poly avec les coordonnées x,y de la souris
                polygon[pointsnumber].x = Input.mousePosition.x;
                polygon[pointsnumber].y = Input.mousePosition.y;
                //crée un cube pour afficher la position du point
                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube.transform.position = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
                //cube.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                pointsnumber++;
                //quand on a les 4 points
                if (pointsnumber == 4)
                {
                    //trie les points pour que le poly soit dans le bon sens
                    SortPoints();
                    //crée les textures contenant les coordonnées UV pour remapper la render texture
                    ConvertToNewCoord();
                    startRender = true;
                }
            }

        }
    }

    //transpose le pixel en position (i,j) dans le plan (u,v)
    // utilise les formules trouvées sur https://math.stackexchange.com/questions/13404/mapping-irregular-quadrilateral-to-a-rectangle/13409#13409
    Vector2 ConvertToUV(Vector2 xy)
    {
        Vector2 _uv = new Vector2();
        _uv.x = Vector2.Dot((xy - polygon[0]), N0) / (Vector2.Dot((xy - polygon[0]), N0) + Vector2.Dot((xy - polygon[2]), N2));
        _uv.y = Vector2.Dot((xy - polygon[0]), N1) / (Vector2.Dot((xy - polygon[0]), N1) + Vector2.Dot((xy - polygon[3]), N3));
        return _uv;
    }
    
    //méthode qui réalise la transformation de la render texture et crée les points correspondants en UV nécessaires au remapping dans le shader
    private void ConvertToNewCoord()
    {
        //calcule les normales du poly à l'aide des formules du post commenté ci-dessus
        N0 = PerpendicularClockwise(polygon[3] - polygon[0]).normalized;
        N1 = PerpendicularCounterClockwise(polygon[1] - polygon[0]).normalized;
        N2 = PerpendicularCounterClockwise(polygon[2] - polygon[1]).normalized;
        N3 = PerpendicularClockwise(polygon[2] - polygon[3]).normalized;
        //instancie les textures qui vont contenir les données de transposition
        texMustDraw = new Texture2D(Screen.width, Screen.height);
        texContainingUVX = new Texture2D(Screen.width, Screen.height);
        texContainingUVY = new Texture2D(Screen.width, Screen.height);
        //pour chaque pixel
        for (int i = 0; i < texMustDraw.width; i++)
        {
            for (int j = 0; j < texMustDraw.height; j++)
            {
                //traite le pixel
                Vector2 pix = new Vector2(i, j);
                //si il est dans le polygone, enregistre sa coordonnées transposée
                if (isInsidePoly(pix, polygon))
                {
                    //récupère les valeurs dans le plan UV
                    Vector2 uv = ConvertToUV(pix);
                    //définit le A à 1 car il faut le dessiner
                    texMustDraw.SetPixel(i, j, new Color(0, 0, 0, 1));
                    //encode UVX en couleur
                    float[] f = EncodeFloatRGBA(uv.x);
                    //donne à la texture contenant les infos de UVX le float dans la couleur du pixel 
                    texContainingUVX.SetPixel(i, j, new Color(f[0], f[1], f[2], f[3]));
                    //pareil pour UVY
                    f = EncodeFloatRGBA(uv.y);
                    texContainingUVY.SetPixel(i, j, new Color(f[0], f[1], f[2], f[3]));

                }
                //sinon, enregistre un pixel vide
                else
                {
                    texMustDraw.SetPixel(i, j, new Color(0, 0, 0, 0));
                    texContainingUVY.SetPixel(i, j, new Color(0, 0, 0, 0));
                    texContainingUVX.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }
        }
        
        //applique les modifications réalisées sur les textures
        texMustDraw.Apply();
        texContainingUVX.Apply();
        texContainingUVY.Apply();
        //envoi les textures au shader juste une fois car pas besoin de recalculer les transpositions
        imgobject.GetComponent<CanvasRenderer>().GetMaterial().SetTexture("_DataTex", texMustDraw);
        imgobject.GetComponent<CanvasRenderer>().GetMaterial().SetTexture("_xTex", texContainingUVX);
        imgobject.GetComponent<CanvasRenderer>().GetMaterial().SetTexture("_yTex", texContainingUVY);
    }
    
    //méthode permettant de convertir un float en 4 octets pour les mettre dans une couleur
    static float[] EncodeFloatRGBA(float val)
    {
        float[] kEncodeMul = new float[] { 1.0f, 255.0f, 65025.0f, 160581375.0f };
        float kEncodeBit = 1.0f / 255.0f;
        for (int i = 0; i < kEncodeMul.Length; ++i)
        {
            kEncodeMul[i] *= val;
            kEncodeMul[i] = (float)(kEncodeMul[i] - Math.Truncate(kEncodeMul[i]));
        }
        
        float[] yzww = new float[] { kEncodeMul[1], kEncodeMul[2], kEncodeMul[3], kEncodeMul[3] };
        for (int i = 0; i < kEncodeMul.Length; ++i)
        {
            kEncodeMul[i] -= yzww[i] * kEncodeBit;
        }

        return kEncodeMul;
    }

    //méthode qui trie les 4 coins du poly pour respecter l'ordre de la conversion UV
    //p0 en bas à gauche / p1 en bas à droite / p2 en haut à droite / p3 en haut à gauche
    void SortPoints()
    {
        int meanX = (int)(polygon[0].x + polygon[1].x + polygon[2].x + polygon[3].x) / 4;
        int meanY = (int)(polygon[0].y + polygon[1].y + polygon[2].y + polygon[3].y) / 4;
        Vector2[] tempPoints = new Vector2[4];
        System.Array.Copy(polygon, tempPoints, 4);
        foreach (Vector2 point in tempPoints)
        {
            float pX = point.x;
            float pY = point.y;
            if (pX < meanX && pY < meanY)
            {
                polygon[0] = point;
            }
            else if (pX > meanX && pY < meanY)
            {
                polygon[1] = point;
            }
            else if (pX > meanX && pY > meanY)
            {
                polygon[2] = point;
            }
            else if (pX < meanX && pY > meanY)
            {
                polygon[3] = point;
            }
            else
            {
                Debug.LogErrorFormat("Error while sorting points");
            }
        }
    }
    
    //méthode qui vérifie si le point v se situe ou non à l'intérieur du poly p
    //but : connaitre les points à redessiner de la texture
    public bool isInsidePoly(Vector2 v, Vector2[] p)
    {
        int j = p.Length - 1;
        bool c = false;
        for (int i = 0; i < p.Length; j = i++) c ^= p[i].y > v.y ^ p[j].y > v.y && v.x < (p[j].x - p[i].x) * (v.y - p[i].y) / (p[j].y - p[i].y) + p[i].x;
        return c;
    }

    //gère la création des coins du poly
    private void Update()
    {
        //tant que le rendu n'a pas commencé, récupère les points du poly
        if (!startRender)
        {
            GetPoints();
        }
    }

    //détruit les textures à la fermeture de l'application
    private void OnApplicationQuit()
    {
        Destroy(texMustDraw);
        Destroy(texContainingUVX);
        Destroy(texContainingUVY);
    }
}