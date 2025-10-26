# 🎰 Bede Lottery Game – .NET Console Application

A simplified lottery game built in **C# (.NET 8)**.

---

## 🧭 Overview

This project simulates a configurable lottery system with both human and CPU players.  
Each player buys tickets, the system draws random winners across multiple prize tiers, and the results are printed to the console.

The implementation emphasizes:
- **Clean architecture**
- **Configurable setup**
- **Extensible design**
- **Testability** via abstraction and mocking

---

## Project Structure

| Project | Responsibility |
|----------|----------------|
| **BedeLotteryConsole** | Console entry point and presentation layer |
| **Application** | Business logic and application services |
| **Domain** | Core entities (players, tickets, prizes) and value objects |
| **Contracts** | Interfaces, factories, and service contracts |
| **Infrastructure** | In-memory repositories and factories |
| **Tests** | Unit and integration tests validating logic correctness |


## ⚙️ Configuration

The game is fully configurable via `appsettings.json`:

```json
{
  "DefaultLotteryConfiguration": {
    "PlayerStartCount": 1,
    "MinPlayers": 10,
    "MaxPlayers": 15,
    "MinTicketsPerPlayer": 1,
    "MaxTicketsPerPlayer": 10,
    "StartingBalance": 10,
    "TicketPrice": 1,
    "Prizes": [
      {
        "Name": "Grand Prize",
        "TicketPercentage": 0,
        "RevenuePercentage": 50,
        "FixedTicketCount": 1,
        "Order": 1
      },
      {
        "Name": "Second Tier",
        "TicketPercentage": 10,
        "RevenuePercentage": 30,
        "Order": 2
      },
      {
        "Name": "Third Tier",
        "TicketPercentage": 20,
        "RevenuePercentage": 10,
        "Order": 3
      }
    ]
  }
}
```

### 🔧 You can easily modify:

- Number of players and tickets  
- Ticket price and starting balance  
- Prize tiers (percentages, order, or logic)  
- Winner count calculation via the strategy pattern  

No code changes required.

---

## 🚀 How to Run


1. Open the solution in **Visual Studio**  
2. Ensure **`BedeLotteryConsoleUI`** is set as the **Startup Project**  
3. Press **F5** (Run)

---

## Extensibility

| Extension Point | Description |
|------------------|-------------|
| **Prize Distribution Strategy** | Add new strategies via `IPrizeDistributionStrategy` |
| **Player Types** | Extend `Player` base class (e.g., BotPlayer, NetworkPlayer) |
| **Repositories** | Swap in-memory repo with DB or cloud-backed repo |
| **Observers** | Implement new `IGameEventListener` for logging or analytics |
| **UI Layer** | Replace console with web, desktop, or API front-ends |

---

## Testing

### Existing Unit Tests
- `Entitytests` – entity object tests
- `LotteryServiceTests` – full game flow, profit, and fairness  

### Run Tests
```bash
dotnet test
```
## 💡 Potential Enhancements

- Add persistence (e.g., EF Core or file storage)  
- Include historical game statistics   
- Use async Publish/Subscribe for events if exposed to external service.
- Make Multi-Player 
