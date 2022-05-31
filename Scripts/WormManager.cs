using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Worms
{
   public string name;
   public GameObject wormObj;
   public Worm script;
   public Team team;
  
 
    public void Set<T,typ>(T obj,Type type)
	{
        switch(type)
		{
            case Type.name:
				{
                    script.name.text = (string)(object)obj;
                    name = (string)(object)obj;
                    break;
				}
            case Type.health:
				{
                    script.SetHealth((float)(object)obj);
                    break;
				}
            case Type.team:
				{
                    script.team = (Worm.Team)(object)obj;
                    break;
				}

         
		}
	}
  
}

public enum Team
{
    red, blue
}
public enum Type
{
    name, health, team
}


public class WormManager : MonoBehaviour
{
    public List<Worms> redTeam;
    public List<Worms> blueTeam;
    public List<string> wormName;
    public float health;
    public GameObject activeWorm,lastWorm;
    public WormUI ui;
    public RoundManager rm;
    // Start is called before the first frame update
    void Start()
    {
        health = 50;
        for(int a=0,b=0; a< wormName.Count;a++)
		{
            if (a < wormName.Count / 2)
            {
                redTeam[a].Set<string, Type>(wormName[a], Type.name);
                redTeam[a].Set<Team, Type>(Team.red, Type.team);
                redTeam[a].Set<float, Type>(health, Type.health);
            }
            else
            {
                blueTeam[b].Set<string, Type>(wormName[a], Type.name);
                blueTeam[b].Set<Team, Type>(Team.blue, Type.team);
                blueTeam[b].Set<float, Type>(health, Type.health);
                b++;
            }
            
		}
        
    }
        

    public void SelectNewWorm(GameObject worm)
	{
        activeWorm = worm;
        lastWorm = worm;
    }

    public void Delete(GameObject w)
    {
        switch (w.GetComponent<Worm>().team)
        {
            case Worm.Team.red:
               redTeam.Remove( Get(w));
                break;
            case Worm.Team.blue:
               blueTeam.Remove( Get(w));
                break;

        }
    }

    public Worms Get(GameObject g )
    {
        switch (g.GetComponent<Worm>().team)
        {
            case Worm.Team.red:
              
                foreach (Worms c in redTeam)
                    {
                    if (c.wormObj == g) return c;
                    } return null;
             
             
            case Worm.Team.blue:
			
                foreach (Worms c in blueTeam)
                {
                    if (c.wormObj == g) return c;

                } return null;
               

            default:
                return new Worms();
        }
        
    }

    public GameObject GetFromActiveTeamByNumber(int number,RoundManager.TeamRound team)
	{
        switch (team)
        {
            case RoundManager.TeamRound.red:
               return redTeam[number].wormObj;
              
            case RoundManager.TeamRound.blue:
               return blueTeam[number].wormObj;
            default: return new GameObject();

        }
    }
}
