export interface FeedbackModel {
  id: string;
  visibility: number;
  category: number;
  title: string | null;
  message: string;

  isAnonymous: boolean;
  displayName: string | null;

  status: number;
  statusMessage: string | null;

  votesCount: number;
  createdAtUtc: Date;
  updatedAtUtc: Date | null;
  updatedByDiscordName: string | null;
}
