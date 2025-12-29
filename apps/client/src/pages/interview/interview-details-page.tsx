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
                            {interview.status === 'Scheduled' && 'Upcoming'}
                            {interview.status === 'NotScheduled' &&
                                'To Be Scheduled'}
                            {interview.status === 'Completed' && 'Completed'}
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
                        sidebarState === 'opened' ? '80px' : '0px'
                    })`,
                }}
            >
                <div className="flex-[50%] px-5 pt-8 space-y-6 ">
                    <div className="space-y-4">
                        {/* <h3 className="font-semibold text-lg">Overview</h3> */}

                        <div className="flex flex-col gap-2">
                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Scheduled At
                                </span>

                                {interview.scheduledAt ? (
                                    <span className="text-sm">
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
                                    Meeting Link
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
                            <div></div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Candidate
                                </span>
                                <a
                                    onClick={() =>
                                        window.open(
                                            `/candidates/candidate/${interview?.candidateId}`,
                                            '_blank',
                                            'noopener,noreferrer'
                                        )
                                    }
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex w-fit items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">
                                        {interview.candidateName}
                                    </span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>

                            <div className="grid grid-cols-[180px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Job Application
                                </span>
                                <a
                                    onClick={() =>
                                        window.open(
                                            `/job-applications/application/${interview?.jobApplicationId}`,
                                            '_blank',
                                            'noopener,noreferrer'
                                        )
                                    }
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="inline-flex w-fit items-center gap-1 text-sm underline hover:cursor-pointer"
                                >
                                    <span className="text-sm">
                                        View Job Application
                                    </span>
                                    <ExternalLink className="w-4 h-4" />
                                </a>
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
                    <div className="space-y-4">
                        <div className="flex items-center justify-between">
                            <h3 className="font-semibold text-lg">
                                Interview Feedbacks
                            </h3>

                            <AddFeedbackDialog
                                interviewId={interview.id}
                                candidateId={interview.candidateId}
                            />
                        </div>

                        <div className="space-y-3 max-h-[600px] overflow-y-auto ">
                            {interview.feedbacks.length === 0 && (
                                <p className="text-center py-10 text-muted-foreground">
                                    No feedbacks yet.
                                </p>
                            )}

                            {interview.feedbacks.map((f) => (
                                <div
                                    key={f.id}
                                    className="border rounded-lg p-4 space-y-3"
                                >
                                    <div className="flex items-start justify-between gap-4">
                                        <div className="flex-1 space-y-1">
                                            <div className="flex items-center gap-2">
                                                <div className="text-base font-semibold">
                                                    {f.rating}/10
                                                </div>
                                            </div>
                                        </div>
                                        <div className="text-right text-xs text-muted-foreground">
                                            <div className="text-xs text-muted-foreground space-y-0.5">
                                                <p>
                                                    Given by{' '}
                                                    <span className="font-medium">
                                                        {f.givenByName}
                                                    </span>
                                                </p>
                                            </div>
                                        </div>
                                    </div>

                                    {f.comment && (
                                        <p className="text-sm leading-relaxed">
                                            {f.comment}
                                        </p>
                                    )}

                                    {f.skillFeedbacks.length > 0 && (
                                        <div className="space-y-2 pt-2 border-t">
                                            <p className="text-xs font-semibold text-muted-foreground">
                                                Skills Assessed
                                            </p>
                                            <div className="flex flex-wrap gap-2">
                                                {f.skillFeedbacks.map((x) => (
                                                    <div
                                                        key={x.skillName}
                                                        className="border rounded-md p-2.5 space-y-1 bg-muted/30"
                                                    >
                                                        <p className="font-medium text-sm">
                                                            {x.skillName}
                                                        </p>
                                                        <div className="flex justify-between text-xs text-muted-foreground">
                                                            <span>
                                                                Rating:{' '}
                                                                <span className="font-semibold">
                                                                    {x.rating}
                                                                    /10
                                                                </span>
                                                            </span>
                                                        </div>
                                                        <div className="text-xs text-muted-foreground">
                                                            Assessed Exp.:{' '}
                                                            <span className="font-semibold">
                                                                {
                                                                    x.assessedExpYears
                                                                }{' '}
                                                                Year
                                                            </span>
                                                        </div>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>
                                    )}
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};
