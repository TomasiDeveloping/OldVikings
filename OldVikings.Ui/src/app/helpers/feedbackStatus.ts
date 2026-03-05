export type FeedbackStatus =
  | 'New'
  | 'UnderReview'
  | 'Planned'
  | 'Implemented'
  | 'Rejected'
  | 'Archived';

export const FeedbackStatusCode: Record<FeedbackStatus, number> = {
  New: 0,
  UnderReview: 1,
  Planned: 2,
  Implemented: 3,
  Rejected: 4,
  Archived: 5
};
