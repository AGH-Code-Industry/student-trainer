using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class NPC : MonoBehaviour
{
    public npcData data;
    private TimeManager timeManager;
    private List<Task> routines;
    private npcMovement npcMovement;
    [HideInInspector]
    public List<Task> routine;
    [HideInInspector]
    public bool canMove = true;
    public bool stationary = false;
    void Start()
    {
        routines = FindObjectOfType<RoutineManager>().routines.ToList();
        timeManager = FindObjectOfType<TimeManager>();
        if (!stationary)
            routine = GenerateRoutine();
        npcMovement = GetComponent<npcMovement>();
    }
    void Update()
    {
        if (stationary)
            return;
        if (routine.Count == 0)
        {
            routine = GenerateRoutine();
        }
        else if (routine[0].startTime.CompareTo(timeManager.globalTime) <= 0 && !npcMovement.isMoving)
        {
            npcMovement.Move(routine[0]);
            routine.RemoveAt(0);
            npcMovement.isMoving = true;
        }
    }
    List<Task> GenerateRoutine()
    {
        List<Task> newRoutine = new List<Task>();
        List<Task> tempRoutines = new List<Task>(routines);
        int amountOfTasks = Random.Range(0, tempRoutines.Count + 1);
        for (int i = 0; i < amountOfTasks; i++)
        {
            int index = Random.Range(0, tempRoutines.Count);
            newRoutine.Add(GenerateTask(tempRoutines[index]));
            tempRoutines.RemoveAt(index);
        }
        newRoutine.Sort((task1, task2) => task1.startTime.CompareTo(task2.startTime));
        return newRoutine;
    }
    Task GenerateTask(Task templateTask)
    {
        Task newTask = new Task();
        Daytime taskDayTime = new Daytime(templateTask.startTime.Days, templateTask.startTime.Hours, templateTask.startTime.Minutes);
        taskDayTime.AddRandomDelay(templateTask.randomTimeDelay);
        newTask.startTime = taskDayTime;
        newTask.destination = templateTask.destination;
        newTask.taskName = templateTask.taskName;
        return newTask;
    }
}

