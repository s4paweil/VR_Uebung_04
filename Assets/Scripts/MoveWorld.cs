using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveWorld : MonoBehaviour
{

    // Hindernislisten für die verschiedenen Positionen
    public List<GameObject> bottom_leftObstacles;
    public List<GameObject> bottom_middleObstacles;
    public List<GameObject> bottom_rightObstacles;
    public List<GameObject> middle_leftObstacles;
    public List<GameObject> middle_middleObstacles;
    public List<GameObject> middle_rightObstacles;
    public List<GameObject> top_leftObstacles;
    public List<GameObject> top_middleObstacles;
    public List<GameObject> top_rightObstacles;
    public List<GameObject> noObstacles;

    // Liste mit möglichen Inputs
    private List<string> possibleInputs = new List<string> { "up", "down", "left", "right" };

    // Die (korrekte) Position des Charakters auf der 3x3 Ebene (angenommen die Mitte ist (0,0))
    private int currentX;
    private int currentY;
    
    // Liste der Inputs
    private List<string> nextInputs;
    
    // Counter um Elemente zwischen Hindernissen einzufügen (gibt an wie viele Blöcke zwischen Hindernissen sein sollen)
    private int obstacleSpace = 4;
    private int currentObstacleSpace = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        int desiredListLength = 10; // Länge der Liste der zufälligen Inputs

        // Erzeuge die Liste mit zufälligen Inputs der gewünschten Länge
        nextInputs = GenerateRandomInputs(desiredListLength);
        
        // Beispiel: Gib die erzeugte Liste der zufälligen Inputs aus
        /*
        foreach (string input in nextInputs)
        {
            Debug.Log("Next Input: " + input);
        }
        */

        // Setze Charakter auf Ausgangsposition (Mitte bei (0,0))
        currentX = 0;
        currentY = 0;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            moveWorld();
        }
    }

    void moveWorld()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.transform.Translate(0,0,-1);
            if (child.transform.position.z < -1)
            {
                Destroy(child.gameObject);
            }
        }

        addElement();

    }

    void addElement()
    {
        GameObject obstacle = null;
        

        if (currentObstacleSpace > 0)
        {
            obstacle = GetRandomObstacle(noObstacles);
            currentObstacleSpace--;
        }

        if (currentObstacleSpace == 0 && nextInputs.Count > 0)
        {
            // Hole den nächsten korrekten Input aus der Liste
            string nextInput = nextInputs[0];
            // Entferne den aktuellen Input aus der Liste, um zum nächsten zu gehen
            nextInputs.RemoveAt(0);
            
            // Bewege den Charakter entsprechend des Inputs
            switch (nextInput)
            {
                case "up":
                    currentY++;
                    break;
                case "down":
                    currentY--;
                    break;
                case "right":
                    currentX++;
                    break;
                case "left":
                    currentX--;
                    break;
            }

            // Wähle das entsprechende Hindernis gemäß der Position des Charakters aus
            switch (currentX)
            {
                case -1:
                    switch (currentY)
                    {
                        case -1:
                            obstacle = GetRandomObstacle(bottom_leftObstacles);
                            break;
                        case 0:
                            obstacle = GetRandomObstacle(middle_leftObstacles);
                            break;
                        case 1:
                            obstacle = GetRandomObstacle(top_leftObstacles);
                            break;
                    }
                    break;
                case 0:
                    switch (currentY)
                    {
                        case -1:
                            obstacle = GetRandomObstacle(bottom_middleObstacles);
                            break;
                        case 0:
                            obstacle = GetRandomObstacle(middle_middleObstacles);
                            break;
                        case 1:
                            obstacle = GetRandomObstacle(top_middleObstacles);
                            break;
                    }
                    break;
                case 1:
                    switch (currentY)
                    {
                        case -1:
                            obstacle = GetRandomObstacle(bottom_rightObstacles);
                            break;
                        case 0:
                            obstacle = GetRandomObstacle(middle_rightObstacles);
                            break;
                        case 1:
                            obstacle = GetRandomObstacle(top_rightObstacles);
                            break;
                    }
                    break;
            }

            currentObstacleSpace = obstacleSpace;
        }

        if (nextInputs.Count == 0)
        {
            obstacle = GetRandomObstacle(noObstacles);
        }

        if (obstacle != null)
        {
            Instantiate(obstacle, new Vector3(0, 0, transform.childCount-2), Quaternion.identity, transform);
        }
    }
    
    private GameObject GetRandomObstacle(List<GameObject> obstacles)
    {
        if (obstacles.Count == 0)
        {
            return null;
        }
        
        int randomIndex = Random.Range(0, obstacles.Count);
        return obstacles[randomIndex];
    }
    
    // Funktion, um eine Liste mit zufälligen Inputs der gewünschten Länge zu generieren
    private List<string> GenerateRandomInputs(int desiredListLength)
    {
        List<string> randomInputs = new List<string>();

        while (randomInputs.Count < desiredListLength)
        {
            // Generiere einen zufälligen Index für die möglichen Inputs
            int randomIndex = Random.Range(0, possibleInputs.Count);

            // Berechne die neue Position, falls der Charakter sich entsprechend des zufälligen Inputs bewegen würde
            int newX = currentX;
            int newY = currentY;

            switch (possibleInputs[randomIndex])
            {
                case "up":
                    newY++;
                    break;
                case "down":
                    newY--;
                    break;
                case "right":
                    newX++;
                    break;
                case "left":
                    newX--;
                    break;
            }

            // Überprüfe, ob die neue Position innerhalb der Grenzen der 3x3 Ebene liegt
            if (newX >= -1 && newX <= 1 && newY >= -1 && newY <= 1)
            {
                // Füge den zufälligen Input der Liste hinzu und aktualisiere die aktuelle Position
                randomInputs.Add(possibleInputs[randomIndex]);
                currentX = newX;
                currentY = newY;
            }
        }

        return randomInputs;
    }
}