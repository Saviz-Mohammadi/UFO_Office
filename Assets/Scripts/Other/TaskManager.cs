using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [System.Serializable]
    public class Task
    {
        public string description;
        public IInteractable interactable;
    }

    [SerializeField] private Text taskTextUI;  // Assign this in the Inspector
    [SerializeField] private float delayBeforeNextTask = 4f;
    
    [SerializeField] private List<Task> availableTasks = new List<Task>();
    private Task currentTask;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AssignRandomTask();
    }

    public void AssignRandomTask()
    {
        if (availableTasks.Count == 0)
        {
            taskTextUI.text = "No tasks available!";
            return;
        }

        // Select a random task
        currentTask = availableTasks[Random.Range(0, availableTasks.Count)];

        // Update UI
        taskTextUI.text = currentTask.description;

        // Show locator
        if (currentTask.interactable != null)
        {
            currentTask.interactable.SetLocatorState(true);
            currentTask.interactable.OnInteracted.AddListener(TaskCompleted);
        }
    }

    private void TaskCompleted()
    {
        if (currentTask == null) return;

        taskTextUI.text = "Task Completed!";
        currentTask.interactable.SetLocatorState(false);
        currentTask.interactable.OnInteracted.RemoveListener(TaskCompleted);
        // Move current task to the end of the list
        availableTasks.Remove(currentTask);
        availableTasks.Add(currentTask);

        // Wait before assigning a new task
        StartCoroutine(WaitBeforeNewTask());
    }

    private IEnumerator WaitBeforeNewTask()
    {
        yield return new WaitForSeconds(delayBeforeNextTask);
        AssignRandomTask();
    }

    public void AddTask(string description, IInteractable interactable)
    {
        availableTasks.Add(new Task { description = description, interactable = interactable });
    }
}
