export interface TrainConductorModel {
  playerId: string;
  playerName: string;
  isAvailable: boolean;
  blockNextCycle: boolean;
  forcePick: boolean;
  updatedAt: Date;
}
