import { useVerifyCandidateBg } from '@/api/candidate-api';
import { Button } from '@/components/ui/button';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import {
    AlertDialog,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from '../ui/alert-dialog';

const verifyCandidateBgFormSchema = z.object({
    candidateId: z.string().nonempty('Candidate ID is required'),
});

type Props = {
    candidateId: string;
};

export function VerifyCandidateBg({ candidateId }: Props) {
    const [open, setOpen] = useState(false);
    const verifyCandidateBgMutation = useVerifyCandidateBg();

    const form = useForm<z.infer<typeof verifyCandidateBgFormSchema>>({
        resolver: zodResolver(verifyCandidateBgFormSchema),
    });

    useEffect(() => {
        form.setValue('candidateId', candidateId);
    }, [candidateId]);

    const onSubmit = async (data: { candidateId: string }) => {
        verifyCandidateBgMutation.mutate(data.candidateId, {
            onSuccess: () => {
                form.reset();
                setOpen(false);
            },
        });
    };

    return (
        <AlertDialog open={open} onOpenChange={setOpen}>
            <AlertDialogTrigger asChild>
                <Button variant="outline" type="button">
                    Verify Background
                </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <form onSubmit={form.handleSubmit(onSubmit)}>
                    <AlertDialogHeader>
                        <AlertDialogTitle>
                            Are you absolutely sure?
                        </AlertDialogTitle>
                        <AlertDialogDescription>
                            You are verifieng the background of the candidate
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                        <Button
                            type="submit"
                            variant="destructive"
                            disabled={verifyCandidateBgMutation.isPending}
                        >
                            Continue
                        </Button>
                    </AlertDialogFooter>
                </form>
            </AlertDialogContent>
        </AlertDialog>
    );
}
