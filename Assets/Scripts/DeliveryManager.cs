using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer = 4f;
    // 4s co 1 recipe
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesAmount;
    private int playerScore;
    private int missedRecipesAmount;

    private void Awake()
    {
        Instance = this;

        successfulRecipesAmount = 0;
        missedRecipesAmount = 0;
        playerScore = 0;

        waitingRecipeSOList = new List<RecipeSO>();

    }
    // rpc mean remote procedure call
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }



        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                int waitingRecipeSOIndex = recipeListSO.GetRandomRecipeSOIndex();
                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
            }

        }
    }
    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {

        RecipeSO waitingRecipeSO = Instantiate(recipeListSO.recipeSOList[waitingRecipeSOIndex]);

        float timeLimit = waitingRecipeSO.timeLimit;

        waitingRecipeSOList.Add(waitingRecipeSO);

        // đếm ngược, hết thời gian thì xóa ra khỏi list 
        StartCoroutine(DestroyRecipe(waitingRecipeSO, timeLimit));

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
    private IEnumerator DestroyRecipe(RecipeSO waitingRecipeSO, float timeLimit)
    {
        // time limit đếm ngược xong thì xóa ra khỏi list
        yield return new WaitForSeconds(timeLimit);
        if (waitingRecipeSOList.Contains(waitingRecipeSO))
        {
            waitingRecipeSOList.Remove(waitingRecipeSO);
            if (playerScore < 0)
            {
                playerScore = 0;
            }
            missedRecipesAmount++; 
            // OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    int point = waitingRecipeSO.points;
                    DeliverCorrectRecipeServerRpc(i, point);
                    return;
                }
            }
            // Player did not deliver the correct recipe
            DeliverInCorrectRecipeServerRpc();
        }

    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliverInCorrectRecipeServerRpc()
    {
        DeliverInCorrectRecipeClientRpc();
    }
    [ClientRpc]
    private void DeliverInCorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSOListIndex, int point)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOListIndex, point);
    }
    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOListIndex, int point)
    {
        waitingRecipeSOList.RemoveAt(waitingRecipeSOListIndex);
        successfulRecipesAmount++;
        playerScore += point;
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
    public int GetPlayerScore()
    {
        return playerScore;
    }
    public int GetMissedRecipesAmount()
    {
        return missedRecipesAmount;
    }
}
