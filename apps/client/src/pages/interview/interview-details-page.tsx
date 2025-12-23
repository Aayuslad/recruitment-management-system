import { useGetInterview } from '@/api/interviews-api';
import { AddFeedbackDialog } from '@/components/interviews/internal/add-feedback-dialog';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Button } from '@/components/ui/button';
import {
    Item,
    ItemActions,
    ItemContent,
    ItemDescription,
    ItemGroup,
    ItemMedia,
    ItemTitle,
} from '@/components/ui/item';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { Spinner } from '@/components/ui/spinner';
import { useAppStore } from '@/store';
import { durationFormatConverter } from '@/util/interview-round-duration-format-converter';
import { ExternalLink } from 'lucide-react';
import React from 'react';
import { useParams } from 'react-router';
import { useShallow } from 'zustand/react/shallow';
import { MarkCompletedButton } from './actions-components/mark-completed-button';
import { ScheduleInterviewDialog } from './actions-components/schedule-interview-dialog';

export const InterviewDetailsPage = () => {
    const { id } = useParams<{ id: string }>();
    const { sidebarState } = useAppStore(
        useShallow((s) => ({
            sidebarState: s.sidebarState,
        }))
    );
    const { data: interview, isLoading, isError } = useGetInterview(id!);

    if (!interview || isLoading)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                <Spinner className="size-8" />
            </div>
        );

    if (isError)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                Error fetching data
            </div>
        );

    return (
        <div className="h-full flex flex-col mb-10">
            <div className="h-30 flex items-center px-10 border-b">
                <div className="space-y-1">
                    <div className="flex space-x-2">
                        <h1 className="text-2xl font-bold">Interview</h1>{' '}
                        <span className="px-3 py-1 rounded-full text-sm font-semibold border">
                            {interview.status}
                        </span>
                    </div>

                    <div className="flex items-center gap-3 text-sm text-muted-foreground">
                        <div>{interview.candidateName}</div>
                        <span>•</span>
                        <span>
                            {interview.roundNumber}. {interview.interviewType}{' '}
                            Round
                        </span>
                        <span>•</span>
                        <span>{interview.designationName}</span>
                    </div>
                </div>

                <div className="ml-auto flex items-center gap-3">
                    {interview.status === 'NotScheduled' && (
                        <ScheduleInterviewDialog interviewId={interview.id} />
                    )}
                    {interview.status === 'Scheduled' && (
                        <MarkCompletedButton interviewId={interview.id} />
                    )}
                </div>
            </div>

            <div
                className="h-full flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${
                        sidebarState === 'opend' ? '80px' : '0px'
                    })`,
                }}
            >
                <div className="flex-[50%] px-5 pt-8 space-y-6 ">
                    <div className="space-y-4">
                        {/* <h3 className="font-semibold text-lg">Overview</h3> */}

                        <div className="flex flex-col gap-2">
                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Candidate Name
                                </span>
                                <span className="text-sm flex items-center gap-2">
                                    {interview.candidateName}
                                    <a
                                        href={interview.candidateId}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                    >
                                        <ExternalLink className="w-4 h-4" />
                                    </a>
                                </span>
                            </div>

                            <div></div>
                            <div></div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Job Application
                                </span>
                                <a
                                    href={interview.jobApplicationId}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">
                                        View Job Application
                                    </span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Designation
                                </span>
                                <span className="text-sm">
                                    {interview.designationName}
                                </span>
                            </div>

                            <div></div>
                            <div></div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Round Details
                                </span>

                                <span className="text-sm">
                                    {interview.roundNumber}.{' '}
                                    {interview.interviewType} Round
                                </span>
                            </div>

                            <div></div>
                            <div></div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Sheduled At
                                </span>

                                {interview.scheduledAt ? (
                                    <span>
                                        {new Date(
                                            interview.scheduledAt
                                        ).toLocaleString()}
                                    </span>
                                ) : (
                                    <span className="text-sm">-</span>
                                )}
                            </div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Duration
                                </span>

                                <span className="text-sm">
                                    {durationFormatConverter(
                                        interview.durationInMinutes
                                    )}
                                </span>
                            </div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Mitting Link
                                </span>

                                {interview.meetingLink ? (
                                    <span className="space-x-3">
                                        <span className="text-sm">
                                            {interview.meetingLink}
                                        </span>
                                        <a
                                            href={interview.meetingLink!}
                                            target="_blank"
                                            rel="noopener noreferrer"
                                            className="inline-flex items-center gap-1 text-sm underline hover:cursor-pointer"
                                        >
                                            <span className="text-sm">
                                                Join
                                            </span>
                                            <ExternalLink className="w-4 h-4" />
                                        </a>
                                    </span>
                                ) : (
                                    <span className="text-sm">-</span>
                                )}
                            </div>

                            <div></div>
                        </div>
                    </div>

                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">Participants</h3>

                        <div className="ml-1">
                            <div className="flex w-full max-w-md flex-col gap-6">
                                <ItemGroup>
                                    {interview.participants.map(
                                        (participant) => (
                                            <React.Fragment
                                                key={participant.userId}
                                            >
                                                <Item className=" py-1.5 px-0">
                                                    <ItemMedia>
                                                        <Avatar>
                                                            {/* <AvatarImage
                                                            src={person.avatar}
                                                            className="grayscale"
                                                        /> */}
                                                            <AvatarFallback>
                                                                {participant.participantUserName.charAt(
                                                                    0
                                                                )}
                                                            </AvatarFallback>
                                                        </Avatar>
                                                    </ItemMedia>
                                                    <ItemContent className="gap-0">
                                                        <ItemTitle>
                                                            {
                                                                participant.participantUserName
                                                            }{' '}
                                                            {/* <span className="text-muted-foreground">
                                                                ({person.email})
                                                            </span> */}
                                                        </ItemTitle>
                                                        <ItemDescription>
                                                            {participant.role}
                                                        </ItemDescription>
                                                    </ItemContent>
                                                    <ItemActions>
                                                        <Button
                                                            variant="ghost"
                                                            size="icon"
                                                            className="rounded-full"
                                                        >
                                                            <ExternalLink />
                                                        </Button>
                                                    </ItemActions>
                                                </Item>
                                            </React.Fragment>
                                        )
                                    )}
                                </ItemGroup>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="flex-[50%] px-5 pt-8 space-y-6 ">
                    <div>
                        <div className="flex items-center justify-between">
                            <h3 className="font-semibold text-lg">
                                Interview Feedbacks
                            </h3>

                            <AddFeedbackDialog
                                interviewId={interview.id}
                                candidateId={interview.candidateId}
                            />
                        </div>

                        <div className="space-y-3">
                            {interview.feedbacks.length === 0 && (
                                <p className="text-center py-10 text-muted-foreground">
                                    No feedbacks yet.
                                </p>
                            )}

                            {interview.feedbacks.map((f) => (
                                <div
                                    key={f.id}
                                    className="border rounded-xl p-3 text-sm space-y-2"
                                >
                                    <div className="flex justify-between">
                                        <span className="font-medium">
                                            Rating: {f.rating} / 10
                                        </span>
                                        <span>
                                            Given by{' '}
                                            <span className="text-muted-foreground">
                                                {f.givenByName}
                                            </span>
                                        </span>
                                    </div>

                                    <p className="text-muted-foreground">
                                        {f.comment}
                                    </p>

                                    <div className="flex flex-wrap">
                                        {f.skillFeedbacks.map((x) => {
                                            return (
                                                <div className="border rounded-sm w-fit text- font-normal m-0.5 py-2 px-2 space-y-0.5">
                                                    <div className="text-center font-semibold">
                                                        {x.skillName}
                                                    </div>
                                                    <div className="font ml-1">
                                                        <span>Rating: </span>
                                                        <span>
                                                            {x.rating}/10
                                                        </span>
                                                    </div>
                                                    <div className="text-sm ml-1">
                                                        <span>
                                                            Assessed Exp.:{' '}
                                                        </span>
                                                        {x.assessedExpYears}{' '}
                                                        Year
                                                    </div>
                                                </div>
                                            );
                                        })}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};
