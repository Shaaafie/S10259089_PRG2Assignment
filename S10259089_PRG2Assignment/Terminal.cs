//==========================================================
// Student Number : S10259089
// Student Name : Nur Shafana Binte Mohd Saktar
// Partner Name : He Zhao Jin
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;

namespace S10259089_PRG2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new();
        public Dictionary<string, Flight> Flights { get; set; } = new();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new();
        public Dictionary<string, double> GateFees { get; set; } = new();

        // New Dictionary to track flight assignments
        public Dictionary<string, string> FlightAssignments { get; set; } = new();

        public Terminal(Dictionary<string, Airline> airlines, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, Flight> flights)
        {
            Airlines = airlines;
            BoardingGates = boardingGates;
            Flights = flights;
        }

        public bool AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines[airline.Code] = airline;
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!BoardingGates.ContainsKey(gate.GateName))
            {
                BoardingGates[gate.GateName] = gate;
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                    return airline;
            }
            return null;
        }

        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                double totalFees = airline.CalculateTotalFees();
                double totalDiscounts = 0;

                foreach (var flight in airline.Flights.Values)
                {
                    totalDiscounts += airline.CalculateDiscounts(flight);
                }

                Console.WriteLine($"{airline.Name}: Total Fees = {totalFees:C}, Discounts = {totalDiscounts:C}, Final Bill = {totalFees - totalDiscounts:C}");
            }
        }

        // Feature 3: Assign a boarding gate to a flight
        public void AssignBoardingGate()
        {
            foreach (var flight in Flights.Values)
            {
                foreach (var gate in BoardingGates.Values)
                {
                    if (gate.Flight == null)  // Only assign an empty gate
                    {
                        // Ensure the gate supports the flight type
                        if (flight is CFFTFlight && !gate.SupportsCFFT) continue;
                        if (flight is DDJBFlight && !gate.SupportsDDJB) continue;
                        if (flight is LWTTFlight && !gate.SupportsLWTT) continue;

                        gate.Flight = flight;
                        FlightAssignments[flight.FlightNumber] = gate.GateName;  // ✅ Store in dictionary
                        break;  // Stop once a gate is assigned
                    }
                }
            }
        }

        // Feature 5: Assign a flight manually
        public void AssignFlightManually(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, string flightsPath)
        {
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine();

            if (flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Flight already exists.");
                return;
            }

            Console.Write("Enter Origin: ");
            string origin = Console.ReadLine();

            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine();

            Console.Write("Enter Expected DateTime (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime expectedTime))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            Console.Write("Enter Status: ");
            string status = Console.ReadLine();

            Flight newFlight = new Flight
            {
                FlightNumber = flightNumber,
                Origin = origin,
                Destination = destination,
                ExpectedTime = expectedTime,
                Status = status
            };

            flights[flightNumber] = newFlight;
            Console.WriteLine($"Successfully added flight {flightNumber}!");

            File.AppendAllText(flightsPath, $"{flightNumber},{origin},{destination},{expectedTime},{status}\n");
        }

        // Feature 6: Create a new flight
        public void CreateNewFlight(Dictionary<string, Flight> flights, string flightsPath)
        {
            while (true)
            {
                Console.Write("Enter flight number: ");
                string flightNumber = Console.ReadLine().Trim();

                if (flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Flight already exists.");
                    continue;
                }

                Console.Write("Enter origin: ");
                string origin = Console.ReadLine().Trim();

                Console.Write("Enter destination: ");
                string destination = Console.ReadLine().Trim();

                Console.Write("Enter expected departure/arrival time (yyyy-MM-dd HH:mm): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime expectedTime))
                {
                    Console.WriteLine("Invalid date format.");
                    continue;
                }

                Console.Write("Do you want to enter a special request code? (Y/N): ");
                string addSpecialRequest = Console.ReadLine().Trim().ToUpper();

                Flight newFlight;

                if (addSpecialRequest == "Y")
                {
                    Console.Write("Enter special request code (DDJB, CFFT, LWTT): ");
                    string specialRequestCode = Console.ReadLine().Trim().ToUpper();

                    switch (specialRequestCode)
                    {
                        case "DDJB":
                            newFlight = new DDJBFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time", RequestFee = 300 };
                            break;
                        case "CFFT":
                            newFlight = new CFFTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time", RequestFee = 150 };
                            break;
                        case "LWTT":
                            newFlight = new LWTTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time", RequestFee = 500 };
                            break;
                        default:
                            Console.WriteLine("Invalid special request code. Creating a normal flight.");
                            newFlight = new NORMFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time" };
                            break;
                    }
                }
                else
                {
                    newFlight = new NORMFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time" };
                }

                flights[flightNumber] = newFlight;

                string newFlightLine = $"{flightNumber},{origin},{destination},{expectedTime:yyyy-MM-dd HH:mm},{newFlight.Status}";
                File.AppendAllText(flightsPath, Environment.NewLine + newFlightLine);

                Console.WriteLine("New flight created and added successfully.");

                Console.Write("Do you want to add another flight? (Y/N): ");
                string addAnother = Console.ReadLine().Trim().ToUpper();

                if (addAnother != "Y")
                {
                    break;
                }
            }

            
        }
        // Feature 9: Display Scheduled Flights in Chronological Order
        public void DisplayScheduledFlights()
        {
            // Sort the flights by ExpectedTime using IComparable implementation
            var sortedFlights = Flights.Values.OrderBy(flight => flight).ToList();

            Console.WriteLine("Scheduled Flights for Today:");

            foreach (var flight in sortedFlights)
            {
                // Find the airline associated with the flight
                var airline = GetAirlineFromFlight(flight);
                string airlineName = airline != null ? airline.Name : "Unknown Airline";

                // Check if the flight has an assigned boarding gate
                string gateAssignment = "Not Assigned";
                foreach (var gate in BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        gateAssignment = gate.GateName;
                        break;
                    }
                }

                // Display the flight information along with airline name, special request code, and boarding gate assignment
                Console.WriteLine($"Flight {flight.FlightNumber}: {airlineName}");
                Console.WriteLine($"Origin: {flight.Origin}, Destination: {flight.Destination}");
                Console.WriteLine($"Scheduled Time: {flight.ExpectedTime:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Status: {flight.Status}");
                Console.WriteLine($"Boarding Gate: {gateAssignment}");
                Console.WriteLine();
            }
        }
        // Advanced feature 1
        public void BulkAssignBoardingGates()
        {
            Queue<Flight> unassignedFlights = new Queue<Flight>();
            HashSet<string> availableGates = new HashSet<string>();
            List<string> assignmentDetails = new List<string>(); // Store assignments

            // Step 1: Collect unassigned flights and available gates
            foreach (var flight in Flights.Values)
            {
                if (!FlightAssignments.ContainsKey(flight.FlightNumber)) // No gate assigned
                {
                    unassignedFlights.Enqueue(flight);
                }
            }

            foreach (var gate in BoardingGates.Values)
            {
                if (gate.Flight == null) // No flight assigned
                {
                    availableGates.Add(gate.GateName);
                }
            }

            Console.WriteLine($"Total Unassigned Flights: {unassignedFlights.Count}");
            Console.WriteLine($"Total Available Boarding Gates: {availableGates.Count}");

            int autoAssignedCount = 0;
            int totalProcessed = unassignedFlights.Count;

            // Step 2: Process flight queue
            while (unassignedFlights.Count > 0 && availableGates.Count > 0)
            {
                Flight flight = unassignedFlights.Dequeue(); // Get first unassigned flight
                string assignedGate = null;

                // Step 3: Find suitable boarding gate
                foreach (var gate in BoardingGates.Values)
                {
                    if (gate.Flight == null) // Check if gate is available
                    {
                        // Match special request code if applicable
                        bool matchesRequest = flight is CFFTFlight && gate.SupportsCFFT ||
                                              flight is DDJBFlight && gate.SupportsDDJB ||
                                              flight is LWTTFlight && gate.SupportsLWTT;

                        // Assign gate based on flight type or general availability
                        if (matchesRequest || !(flight is CFFTFlight || flight is DDJBFlight || flight is LWTTFlight))
                        {
                            assignedGate = gate.GateName;
                            gate.Flight = flight;
                            break;
                        }
                    }
                }

                // Step 4: Assign the gate if found
                if (assignedGate != null)
                {
                    FlightAssignments[flight.FlightNumber] = assignedGate;
                    availableGates.Remove(assignedGate); // Mark gate as used
                    autoAssignedCount++;

                    // Store the assignment detail instead of printing
                    assignmentDetails.Add($"Assigned Flight {flight.FlightNumber} ({flight.Origin} to {flight.Destination}) to Gate {assignedGate}");
                }
            }

            // Step 5: Display final processing summary
            Console.WriteLine($"Total Flights Processed: {totalProcessed}");
            Console.WriteLine($"Total Gates Processed: {BoardingGates.Count}");
            Console.WriteLine($"Automatic Assignments: {autoAssignedCount}/{totalProcessed} ({(totalProcessed > 0 ? (autoAssignedCount * 100 / totalProcessed) : 0)}%)");

            // Step 6: Print all assignments at once
            foreach (var assignment in assignmentDetails)
            {
                Console.WriteLine(assignment);
            }
        }

        // Advanced Feature 2
        public void DisplayTotalFeesPerAirline()
        {
            // Step 1: Ensure all flights have assigned gates
            var unassignedFlights = Flights.Values.Where(f => !FlightAssignments.ContainsKey(f.FlightNumber)).ToList();

            if (unassignedFlights.Any())
            {
                Console.WriteLine("Some flights are not assigned a boarding gate! Please assign all flights before calculating fees.");
                return;
            }

            double totalAirlineFees = 0;
            double totalAirlineDiscounts = 0;
            double finalFeesCollected = 0;

            Console.WriteLine("\n**Total Fees Per Airline for the Day:**\n");

            // Step 2: Process each airline
            foreach (var airline in Airlines.Values)
            {
                double airlineSubtotal = 0;
                double airlineDiscounts = 0;

                Console.WriteLine($"{airline.Name} (Code: {airline.Code})");

                // Step 3: Process each flight
                foreach (var flight in airline.Flights.Values)
                {
                    double flightFee = 0;

                    // Apply SIN fees
                    if (flight.Origin == "SIN") flightFee += 800;
                    if (flight.Destination == "SIN") flightFee += 500;

                    // Apply Boarding Gate Base Fee
                    flightFee += 300;

                    // Apply Special Request Fees
                    if (flight is DDJBFlight) flightFee += 300;
                    if (flight is CFFTFlight) flightFee += 150;
                    if (flight is LWTTFlight) flightFee += 500;

                    // Calculate discounts for the flight
                    double flightDiscount = airline.CalculateDiscounts(flight);

                    airlineSubtotal += flightFee;
                    airlineDiscounts += flightDiscount;

                    Console.WriteLine($"{flight.FlightNumber}: {flight.Origin} → {flight.Destination}, Base Fee: ${flightFee}, Discount: -${flightDiscount}");
                }

                // Step 4: Compute total airline fees
                double finalAirlineFee = airlineSubtotal - airlineDiscounts;
                Console.WriteLine($"Subtotal: ${airlineSubtotal}, Discounts: -${airlineDiscounts}, **Final Fee: ${finalAirlineFee}**\n");

                totalAirlineFees += airlineSubtotal;
                totalAirlineDiscounts += airlineDiscounts;
                finalFeesCollected += finalAirlineFee;
            }

            // Step 5: Display final totals
            double discountPercentage = totalAirlineFees > 0 ? (totalAirlineDiscounts * 100 / totalAirlineFees) : 0;

            Console.WriteLine("\nSummary of Terminal Fees for the Day: ");
            Console.WriteLine($"Total Airline Fees: ${totalAirlineFees}");
            Console.WriteLine($"Total Discounts Applied: ${totalAirlineDiscounts}");
            Console.WriteLine($"Final Fees Collected by Terminal: ${finalFeesCollected}");
            Console.WriteLine($"Discount Percentage: {discountPercentage:F2}%");
        }

        // Additional Feature
        public void RescheduleFlight()
        {
            Console.Write("\nEnter Flight Number to Reschedule: ");
            string flightNumber = Console.ReadLine().Trim();

            if (!Flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Flight not found!");
                return;
            }

            Flight flight = Flights[flightNumber];

            Console.Write("Enter New Expected DateTime (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newTime))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                return;
            }

            Console.WriteLine($"Rescheduling Flight {flight.FlightNumber} to {newTime}...");

            // Check if the flight already has an assigned gate
            if (FlightAssignments.ContainsKey(flightNumber))
            {
                string currentGate = FlightAssignments[flightNumber];
                BoardingGate gate = BoardingGates[currentGate];

                // If the gate is occupied at the new time, reassign
                if (gate.Flight != null && gate.Flight != flight)
                {
                    Console.WriteLine($"Gate {currentGate} is occupied at the new time. Searching for another gate...");
                    AssignBoardingGateForReschedule(flight, newTime);
                }
                else
                {
                    // Update flight time
                    flight.ExpectedTime = newTime;
                    Console.WriteLine($"Flight {flight.FlightNumber} successfully rescheduled to {newTime} at Gate {currentGate}.");
                }
            }
            else
            {
                Console.WriteLine($"Flight {flight.FlightNumber} has no assigned gate. Assigning a new gate...");
                AssignBoardingGateForReschedule(flight, newTime);
            }
        }

        private void AssignBoardingGateForReschedule(Flight flight, DateTime newTime)
        {
            foreach (var gate in BoardingGates.Values)
            {
                if (gate.Flight == null) // Check if the gate is empty
                {
                    // Assign the flight to the gate
                    gate.Flight = flight;
                    FlightAssignments[flight.FlightNumber] = gate.GateName;
                    flight.ExpectedTime = newTime;

                    Console.WriteLine($"Flight {flight.FlightNumber} assigned to Gate {gate.GateName} at {newTime}.");
                    return;
                }
            }

            Console.WriteLine($"No available gates. Flight {flight.FlightNumber} is put on hold.");
        }


        public override string ToString()
        {
            return $"Terminal: {TerminalName}\n" +
                   $"Airlines: {string.Join(", ", Airlines.Values.Select(a => a.Name))}\n" +
                   $"Flights: {string.Join(", ", Flights.Values.Select(f => f.FlightNumber))}\n" +
                   $"BoardingGates: {string.Join(", ", BoardingGates.Values.Select(g => g.GateName))}\n" +
                   $"GateFees: {string.Join(", ", GateFees.Select(g => $"{g.Key}: {g.Value:C}"))}";
        }
    }
}