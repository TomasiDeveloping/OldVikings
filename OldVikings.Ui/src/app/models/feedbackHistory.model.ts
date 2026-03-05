import {FeedbackStatus} from "../helpers/feedbackStatus";

export interface FeedbackHistoryModel {
  changedAtUtc: Date;
  oldStatus: number;
  newStatus: number;
  note: string | null;
  discordUserName: string;
}
