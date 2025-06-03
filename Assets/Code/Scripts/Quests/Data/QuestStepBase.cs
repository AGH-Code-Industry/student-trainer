using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStepBase
{
    protected Quest parentQuest;

    public string stepText { get; protected set; }
    public QuestStepStatus status { get; protected set; }

    public QuestStepBase[] requiredSteps { get; protected set; }
    public QuestStepBase[] nextSteps { get; protected set; }

    public QuestStepBase(Quest parentQuest, string stepText, QuestStepBase[] req, QuestStepBase[] next)
    {
        this.parentQuest = parentQuest;
        this.stepText = stepText;

        status = QuestStepStatus.Waiting;

        requiredSteps = req;
        nextSteps = next;
    }

    public virtual void StepBegin()
    {
        status = QuestStepStatus.Ongoing;
    }

    public abstract void StepUpdate();

    public virtual void StepEnd()
    {
        status = QuestStepStatus.Completed;
    }
}

public enum QuestStepStatus
{
    Waiting,
    Ongoing,
    Completed,
    Failed
}