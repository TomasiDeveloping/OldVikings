using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Dto;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class PlayerRepository(OldVikingsContext dbContext) : IPlayerRepository
{
    public async Task<List<PlayerDto>> GetPlayersAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Players
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                DisplayName = p.DisplayName,
                Registered = p.Registered,
                Approved = p.Approved
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountActiveAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Players.CountAsync(p => p.Registered && p.Approved, cancellationToken);
    }

    public async Task<List<Guid>> GetActivePlayerIdsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Players
            .Where(p => p.Registered)
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Player> CreateAsync(CreatePlayerDto dto, CancellationToken cancellationToken = default)
    {
        var newPlayer = new Player
        {
            DisplayName = dto.PlayerName,
            CreatedAt = DateTime.UtcNow,
            Id = Guid.CreateVersion7(),
            Registered = false
        };

        await dbContext.Players.AddAsync(newPlayer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return newPlayer;
    }

    public async Task SetPlayerActiveAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var player = await dbContext.Players.FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);

        if (player is null) throw new ApplicationException("No Player found");

        player.Registered = true;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DisablePlayerAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var player = await dbContext.Players.FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);

        if (player is null)
        {
            throw new ApplicationException("No player found");
        }
        player.Registered = false;
        player.Approved = false;

        var poolLeader = await dbContext.PoolLeaders.FirstOrDefaultAsync(pl => pl.PlayerId == playerId, cancellationToken);

        if (poolLeader is not null)
        {
            poolLeader.IsAvailable = false;
            poolLeader.UpdatedAt = DateTime.UtcNow;
        }

        var poolVip = await dbContext.PoolVips.FirstOrDefaultAsync(pv => pv.PlayerId == playerId, cancellationToken);
        if (poolVip is not null)
        {
            poolVip.IsAvailable = false;
            poolVip.UpdatedAt = DateTime.UtcNow;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async Task<PlayerDto> UpdatePlayerAsync(UpdatePlayerDto dto, CancellationToken cancellationToken = default)
    {
        var playerToUpdate = await dbContext.Players.FirstOrDefaultAsync(p => p.Id == dto.Id, cancellationToken);

        if (playerToUpdate is null)
        {
            throw new ApplicationException("No player found");
        }

        if (!dto.Approved)
        {
            await DisablePlayerAsync(playerToUpdate.Id, cancellationToken);
            return await GetPlayerAsync(playerToUpdate.Id, cancellationToken);
        }

        playerToUpdate.DisplayName = dto.DisplayName;
        playerToUpdate.Approved = dto.Approved;
        playerToUpdate.Registered = dto.Registered;

        await dbContext.SaveChangesAsync(cancellationToken);

        return await GetPlayerAsync(playerToUpdate.Id, cancellationToken);
    }

    public async Task DeletePlayerAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var player = await dbContext.Players.FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);

        var vipDays = await dbContext.WeeklyScheduleDays.
            Where(p => p.VipPlayerId == playerId)
            .ToListAsync(cancellationToken);

        if (player is null)
        {
            throw new ApplicationException("No player found");
        }

        if (vipDays.Count != 0)
        {
            foreach (var vipDay in vipDays)
            {
                vipDay.VipPlayerId = null;
            }
        }

        var leaderDays = await dbContext.WeeklyScheduleDays
            .Where(p => p.LeaderPlayerId == playerId)
            .ToListAsync(cancellationToken);

        if (leaderDays.Count != 0)
        {
            foreach (var leaderDay in leaderDays)
            {
                leaderDay.LeaderPlayerId = null;
            }
        }

        dbContext.Players.Remove(player);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<PlayerDto> GetPlayerAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var player = await dbContext.Players.FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);
        if (player is null)
        {
            throw new ApplicationException("No player found");
        }
        return new PlayerDto
        {
            Id = player.Id,
            DisplayName = player.DisplayName,
            Registered = player.Registered,
            Approved = player.Approved
        };
    }
}